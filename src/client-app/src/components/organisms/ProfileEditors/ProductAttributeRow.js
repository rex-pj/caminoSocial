import React from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { LabelNormal, LabelDark } from "../../atoms/Labels";
import {
  ButtonOutlinePrimary,
  ButtonOutlineDanger,
  ButtonOutlineLight,
} from "../../atoms/Buttons/OutlineButtons";
import styled from "styled-components";
import ProductAttributeValueRow from "./ProductAttributeValueRow";

const FormRow = styled.div`
  margin-bottom: ${(p) => p.theme.size.tiny};
  background-color: ${(p) => p.theme.color.primaryDivide};
  border-radius: ${(p) => p.theme.borderRadius.normal};

  label {
    vertical-align: middle;
  }
`;

const AttributeValuePanel = styled.div`
  background-color: ${(p) => p.theme.color.secondaryDivide};
  border-bottom-left-radius: ${(p) => p.theme.borderRadius.normal};
  border-bottom-right-radius: ${(p) => p.theme.borderRadius.normal};
`;

export default (props) => {
  const { attribute, onEditAttributeValue } = props;
  const { attributeValues } = attribute;

  const onRemoveAttributeValue = (currentAttributeValue) => {
    let { attribute } = props;
    let { attributeValues } = attribute;
    const index = attributeValues.indexOf(currentAttributeValue);
    attributeValues.splice(index, 1);

    attribute.attributeValues = attributeValues;
    props.onAttributeChange(attribute);
  };

  return (
    <FormRow className="mb-2">
      <div className="row p-2">
        <div className="col-2 col-xl-1">
          <ButtonOutlinePrimary
            type="button"
            size="xs"
            title="Add attribute value"
            onClick={() => props.onAddAttributeValue()}
          >
            <FontAwesomeIcon icon="plus" />
          </ButtonOutlinePrimary>
        </div>
        <div className="col-3 col-xl-3 ps-0 pt-1">
          <LabelNormal className="me-1">Attribute:</LabelNormal>
          <LabelDark>{attribute.attributeId}</LabelDark>
        </div>
        <div className="col-3 col-xl-3 pt-1">
          <LabelNormal className="me-1">Control:</LabelNormal>
          <LabelDark>{attribute.attributeControlTypeId}</LabelDark>
        </div>
        <div className="col-2 col-xl-3 pt-1">
          <LabelNormal className="me-1">Order:</LabelNormal>
          <LabelDark>{attribute.displayOrder}</LabelDark>
        </div>
        <div className="col-auto">
          <ButtonOutlineLight
            type="button"
            size="xs"
            className="me-1"
            title="Edit"
            onClick={() => props.onEditAttribute(attribute)}
          >
            <FontAwesomeIcon icon="pencil-alt" />
          </ButtonOutlineLight>
          <ButtonOutlineDanger
            type="button"
            size="xs"
            title="Remove"
            onClick={() => props.onRemoveAttribute(attribute)}
          >
            <FontAwesomeIcon icon="times" />
          </ButtonOutlineDanger>
        </div>
      </div>
      <AttributeValuePanel>
        {attributeValues
          ? attributeValues.map((attrVal, index) => {
              return (
                <ProductAttributeValueRow
                  className="p-2 row mb-2"
                  key={index}
                  attributeValue={attrVal}
                  onRemoveAttributeValue={onRemoveAttributeValue}
                  onEditAttributeValue={(e) => onEditAttributeValue(e, index)}
                ></ProductAttributeValueRow>
              );
            })
          : null}
      </AttributeValuePanel>
    </FormRow>
  );
};
